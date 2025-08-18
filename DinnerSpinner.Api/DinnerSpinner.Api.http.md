DinnerSpinner.Api.http is a **REST request scratchpad** that ships with the ASP.NET Core Web API template. It lets you send HTTP requests to your API right from the editor—no Postman needed.

* `@DinnerSpinner.Api_HostAddress = http://localhost:5054` defines a variable for your base URL.
* `GET {{DinnerSpinner.Api_HostAddress}}/api/dishes/` is a sample request that uses that variable.
* `###` separates requests inside the same file.
* It doesn’t affect your app at runtime; it’s safe to edit or delete. It’s just for quick testing.

### How to use it

* **Visual Studio (Windows/Mac):** open the `.http` file, run the API, then click **Send Request** above any request line. The response opens in a side pane.
* **VS Code:** install the “REST Client” extension, open the file, then click **Send Request**.

### Make it useful for DinnerSpinner

Replace the sample with your real endpoints (paths as shown in Swagger). For example:

```http
@host = http://localhost:5054
@json = application/json

### List dishes
GET {{host}}/dishes
Accept: {{json}}

###
# Create a dish
POST {{host}}/dishes
Content-Type: {{json}}

{
  "name": "Spaghetti Bolognese",
  "category": "Pasta"
}

###
# Get one dish
GET {{host}}/dishes/1
Accept: {{json}}

###
# Update a dish
PUT {{host}}/dishes/1
Content-Type: {{json}}

{
  "name": "Spaghetti Bolognese",
  "category": "Pasta & Sauce"
}

###
# Delete a dish
DELETE {{host}}/dishes/1
```

Tip: if you add auth later, define a token variable and reuse it:

```http
@token = <paste JWT here>

GET {{host}}/dishes
Authorization: Bearer {{token}}
Accept: {{json}}
```

That’s it—treat the `.http` file like a living, in-repo API test sheet you can run with a click.
