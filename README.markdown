# e-KEP Citizen Service Center Platform

## Overview
This C# desktop application is designed to streamline the management of citizen requests at a Citizen Service Center (KEP). It allows KEP employees to record, view, edit, and delete citizen requests for certificates, attestations, and other services. The application stores data in a SQLite database and supports advanced features like searching requests by specific criteria and exporting them to text or PDF files. Built with a focus on usability and efficiency, it demonstrates robust desktop development and database integration.

## Technologies
- **Language**: C# (.NET Framework)
- **GUI**: Windows Forms or WPF (depending on implementation)
- **Database**: SQLite
- **PDF Export**: iTextSharp or similar library (for PDF generation)
- **Build Tool**: Visual Studio

## Features
- **Data Entry**: Record citizen requests with details like name, email, phone, date of birth, request type, address, and request timestamp.
- **CRUD Operations**:
  - Create: Add new citizen requests.
  - Read: View all requests or filter by citizen.
  - Update: Modify existing request details.
  - Delete: Remove requests from the database.
- **Advanced Search**: Search requests by criteria such as name, request type, or date.
- **Export Functionality**: Export individual requests to text or PDF files for documentation.
- Persistent storage in SQLite for reliable and efficient data management.
- User-friendly GUI for seamless interaction and data management.

## Installation
1. Clone the repository:
   ```
   git clone https://github.com/OzzYBcc/e-KEP-Citizen-Service-Center-Platform.git
   ```
2. Navigate to the project directory:
   ```
   cd e-KEP-Citizen-Service-Center-Platform
   ```
3. Open the solution file (`.sln`) in Visual Studio.
4. Ensure the required NuGet packages are installed (e.g., `System.Data.SQLite` for database operations, `iTextSharp` for PDF export).
5. Set up the SQLite database by running the `setup.sql` script (located in the `/db` directory) to initialize the request table.
6. Build and run the project in Visual Studio:
   ```
   dotnet build
   dotnet run
   ```

## Usage
- **Add Request**: Enter citizen details (name, email, phone, date of birth, request type, address) and submit to store in the database.
- **View Requests**: Display all requests or filter by citizen name or other criteria.
- **Edit/Delete**: Modify or remove existing requests via the GUI.
- **Search**: Use the search feature to find requests by name, request type, or date.
- **Export**: Save a request as a text or PDF file for record-keeping.
- **Example Code (Add Request)**:
  ```csharp
  using System.Data.SQLite;

  public void AddRequest(CitizenRequest request) {
      using (var connection = new SQLiteConnection("Data Source=kep.db;Version=3;")) {
          connection.Open();
          var command = new SQLiteCommand(
              "INSERT INTO Requests (Name, Email, Phone, BirthDate, RequestType, Address, RequestDate) " +
              "VALUES (@name, @email, @phone, @birthDate, @requestType, @address, @requestDate)",
              connection);
          command.Parameters.AddWithValue("@name", request.Name);
          command.Parameters.AddWithValue("@email", request.Email);
          command.Parameters.AddWithValue("@phone", request.Phone);
          command.Parameters.AddWithValue("@birthDate", request.BirthDate);
          command.Parameters.AddWithValue("@requestType", request.RequestType);
          command.Parameters.AddWithValue("@address", request.Address);
          command.Parameters.AddWithValue("@requestDate", request.RequestDate);
          command.ExecuteNonQuery();
      }
  }
  ```
- **Example Code (Export to Text)**:
  ```csharp
  public void ExportToText(CitizenRequest request, string filePath) {
      File.WriteAllText(filePath, 
          $"Name: {request.Name}\nEmail: {request.Email}\nPhone: {request.Phone}\n" +
          $"Birth Date: {request.BirthDate}\nRequest Type: {request.RequestType}\n" +
          $"Address: {request.Address}\nRequest Date: {request.RequestDate}");
  }
  ```

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Commit your changes (`git commit -m 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Open a pull request.

Potential improvements include adding support for bulk exports, enhancing the GUI with modern styling, or integrating email notifications for request updates.

## License
MIT License