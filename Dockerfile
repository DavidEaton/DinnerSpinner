# syntax=docker/dockerfile:1
FROM debian:bookworm-slim

ARG UID=1000
ARG GID=1000

# Base deps:
# - curl + bash: required for the native install script
# - git: common for repo operations
# - ripgrep: commonly used by Claude Code workflows
# - ca-certificates: TLS
# - tini: clean signal handling for interactive CLI
RUN apt-get update && apt-get install -y --no-install-recommends \
    bash \
    ca-certificates \
    curl \
    git \
    ripgrep \
    tini \
  && rm -rf /var/lib/apt/lists/*

# Install Claude Code using Anthropic's native installer script (recommended in docs).
# The script installs into ~/.local/bin/claude; we copy it to /usr/local/bin for all users.
RUN curl -fsSL https://claude.ai/install.sh | bash \
  && if [ -x /root/.local/bin/claude ]; then \
       cp -a /root/.local/bin/claude /usr/local/bin/claude ; \
     else \
       echo "Claude binary not found at /root/.local/bin/claude after install" >&2 ; \
       exit 1 ; \
     fi \
  && chmod 0755 /usr/local/bin/claude \
  && rm -rf /root/.local

# Create a non-root user that matches your host UID/GID when provided.
RUN groupadd -g ${GID} claude \
  && useradd -m -u ${UID} -g ${GID} -s /bin/bash claude

ENV HOME=/home/claude
ENV PATH="/usr/local/bin:${HOME}/.local/bin:${PATH}"

WORKDIR /workspace

# Tini for proper Ctrl-C / signal handling in interactive sessions
ENTRYPOINT ["tini","--"]
CMD ["claude"]
