# Design Notes

CreditWorks Vehicle Management: a web application for recording vehicles and managing the weight categories they belong to.

## 1. Solution architecture

  React app (frontend/)  ---HTTP/JSON--->  ASP.NET Core Web API (backend/)  ---EF Core--->  MS SQL Server

## 2. Validation and error handling

  •⁠  ⁠*Every validations are checked on the server.* ⁠ [Required] ⁠ on the DTOs checks that each field is
    present; the validators in ⁠ Utils/ ⁠ check the business rules; the controller checks that the
    manufacturer exists. The frontend uses only HTML input checks (⁠ required ⁠, ⁠ min ⁠, ⁠ max ⁠,
    ⁠ step="0.01" ⁠) for quick feedback, and never relies on them.
  •⁠  ⁠*Error responses use the standard formats:*
    - *400* validation errors 
    - *404* for a vehicle or category that does not exist.
  •⁠  ⁠*Frontend:* each server message is shown under the matching field. If the server can't be
    reached, the page shows "Cannot reach the server".

## 3. Security

•⁠  ⁠*SQL injection:* all data access goes through EF Core, which sends parameterised queries. Sorting
  uses an enum, not a column name taken from the request.
•⁠  ⁠*Input:* validated on the server, with length limits matching the database columns.
•⁠  ⁠*Secrets:* no real connection string is committed. ⁠ appsettings.Development.json ⁠ contains only a
  placeholder.

## 4. Known limitations

•⁠  ⁠The vehicle list has no paging, all vehicles are loaded.
•⁠  ⁠Manufacturers can only be changed through a migration or directly in the database.

## 5. Improvements

•⁠  ⁠Adding paging for vehicle list
•⁠  ⁠*Caching the categories*, because they change rarely.
•⁠  ⁠Introduce *Authentication and authorisations*.
