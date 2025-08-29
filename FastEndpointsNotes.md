**epreq**
// Scaffolds an endpoint with only a request dto
sealed class Endpoint : Endpoint<Request>
epreqres
// Scaffolds an endpoint with request and response dtos
sealed class Endpoint : Endpoint<Request, Response>
epnoreq
// Scaffolds an endpoint without a request nor response dto
sealed class Endpoint : EndpointWithoutRequest
epres
// Scaffolds an endpoint without a request dto but with a response dto
sealed class Endpoint : EndpointWithoutRequest<Response>