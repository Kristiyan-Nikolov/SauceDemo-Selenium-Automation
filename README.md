# SauceDemo Automated Tests

Automated end-to-end tests for the [SauceDemo](https://www.saucedemo.com/) web application using **C#**, **Selenium WebDriver**, and **NUnit**.

## Author

Kristiyan Nikolov – [GitHub](https://github.com/Kristiyan-Nikolov)

## Overview

This is my first automated testing project using the SauceDemo website. It demonstrates a structured approach with Page Object Model (POM) principles, covering login, inventory, cart, checkout, and item pages with various test scenarios.

### Key Features

- **Page Object Model** for maintainable and reusable test logic.
- Tests grouped by functional areas: Authentication, Inventory, Cart, Checkout, and ItemPage.
- High test coverage, including edge cases and validation scenarios.
- Smoke tests ready for CI/CD integration.
- Sorting, adding/removing items, checkout calculations, and UI validations included.

## Test Structure

- `AuthenticatedTests` – Handles setup, teardown, and driver configuration.
- `BaseTests` – Handles setup, teardown, and driver configuration.
- `LoginTests` – Tests login functionality with various credentials.
- `InventoryTests` – Tests inventory page functionality and sorting.
- `CartTests` – Tests cart badge updates and item persistence.
- `CheckoutTests` – Tests checkout flow and price calculations.
- `InventoryItemTests` – Tests individual item pages and cart interactions.

## Getting Started

### Prerequisites

- .NET 7 SDK or higher
- Firefox browser
- Geckodriver installed and available in your PATH
- NUnit 3 and Selenium WebDriver packages installed via NuGet

### Running Tests Locally

1. Clone the repository:

```bash
git clone https://github.com/Kristiyan-Nikolov/SauceDemo-Selenium-Automation.git
cd SauceDemo
```

2. Restore NuGet packages:

```bash
dotnet restore
```

3. Run all tests:

```bash
dotnet test
```

4. To run specific categories (e.g., "Smoke"):

```bash
dotnet test --filter TestCategory=Smoke
```

### Test Artifacts

- Screenshots are automatically saved in the `Screenshots/` folder on test failure.
- Browser console logs are saved in the `Logs/` folder on test failure.
- Attachments are also added to NUnit reports for easier debugging.

## CI/CD Integration

- Ready for pipeline execution (e.g., GitHub Actions, Azure DevOps, Jenkins).
- Smoke and regression tests can be triggered automatically on pull requests or merges.
- Automatic failure reporting with screenshots and logs.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is open source and available under the MIT License.
