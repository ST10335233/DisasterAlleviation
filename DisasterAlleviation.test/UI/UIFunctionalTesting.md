UI Functional Testing for Disaster Alleviation Application
1. Purpose

Ensure all UI elements function as expected.

Verify forms, navigation, buttons, and error messages work correctly.

Provide a seamless and intuitive experience for volunteers, admins, and donors.

2. Scope

Test UI components for all user roles.

Check form validations and error handling.

Verify proper navigation between pages.

Assess accessibility and usability.

3. Test Environment

Browsers: Chrome, Edge, Firefox

Screen resolution: 1366x768; responsive testing on mobile

Backend: Azure SQL Test Database

Application: Local deployment or Azure App Service (test environment)

4. Test Scenarios
Volunteer Registration

Navigate to the registration page; page loads successfully.

Fill all fields with valid data; submission succeeds and data saved in database.

Submit empty fields; required field validation messages appear.

Enter invalid email; appropriate error message displayed.

Incident Reporting

Navigate to Report Incident page; page loads correctly.

Submit valid incident data; confirmation message appears and incident is listed.

Leave required fields empty; validation messages displayed.

Enter incorrect dates; validation error shown.

Task Assignment (Admin)

Navigate to Task Registration page; page loads correctly.

Assign a task to a volunteer; task appears with correct volunteer and incident title.

Leave required fields empty; validation errors displayed.

Donations

Navigate to Donations page; donation form displayed.

Submit valid donation; success message shown, status = "Pending".

Enter invalid quantity (e.g., negative number); validation error displayed.

Navigation

Test Home, About, Contact links; pages load correctly.

Use menu to navigate between sections; correct pages displayed.

Use browser Back/Forward buttons; navigation works correctly.

5. Usability Testing

Testers: 3–5 fictitious users (volunteers, admins, donors)

Feedback collected on:

Navigation ease

Readability

Form usability

Visual layout

Overall satisfaction

Summary:

Navigation intuitive.

Some error messages need clearer wording.

Forms easy to complete.

6. Tools Used

Browser developer tools for inspection and responsiveness.

Manual testing on Chrome, Edge, and Firefox.

Screenshots and videos to document test outcomes.

7. Test Results

UI components function correctly across browsers.

Form validations work as intended.

Navigation flows tested and successful.

Minor usability improvements noted for future iterations.

8. Conclusion

The Disaster Alleviation application’s UI is functionally sound and user-friendly.

All major flows for volunteers, admins, and donors work correctly.

Usability feedback provides actionable insights for enhancements.