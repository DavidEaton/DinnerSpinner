// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0305:Simplify collection initialization", Justification = "New collection expression syntax reduces clarity in this endpoint; explicit type improves readability and consistency with existing code.", Scope = "member", Target = "~M:DinnerSpinner.Api.Features.Dishes.Read.List.Endpoint.HandleAsync(System.Threading.CancellationToken)~System.Threading.Tasks.Task")]
