Feature: SCRUM116 Theme Switching Management in Application

@mohammed
Scenario: Visibility of the Theme button on the navbar
    Given a user is navigating the application
    When the user views the navbar
    Then the user should see a clearly visible Theme button

Scenario: Accessing theme options via the Theme button
    Given a user clicks on the Theme button in the navbar
    When the Theme dropdown drops
    Then the user should see two distinct buttons labeled "Light Theme" and "Dark Theme"

Scenario: Switching to Dark Theme
    Given a user is on the Theme dropdown
    When the user clicks the "Dark Theme" button
    Then the application should refresh
    And display all pages in dark theme mode with accessible contrasting colors

Scenario: Switching back to Light Theme
    Given a user is on the Theme dropdown and the dark theme is active
    When the user clicks the "Light Theme" button
    Then the application should refresh
    And revert to light theme mode with the original accessible color scheme
