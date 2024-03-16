import '@testing-library/jest-dom';
import { initialize } from '@js/announcementCreateForm';


beforeAll(() => {
  // Setup the necessary DOM elements
  document.body.innerHTML = `
    <form id="createAnnouncementForm">
      <input id="title" />
      <textarea id="description"></textarea>
      <input id="author" />
      <input id="adminKey" />
    </form>
  `;

  // Mock fetch
  global.fetch = jest.fn(() =>
    Promise.resolve({
      ok: true,
      json: () => Promise.resolve({ message: 'Success' }),
    })
  );

  // Initialize the form functionality
  initialize();
});

test('Form submission with valid details triggers fetch', () => {
  // Mock form input values
  document.getElementById('title').value = 'Test Title';
  document.getElementById('description').value = 'Test Description';
  document.getElementById('author').value = 'Test Author';
  document.getElementById('adminKey').value = 'Test Key';

  // Simulate form submission
  document.getElementById('createAnnouncementForm').dispatchEvent(new Event('submit'));

  // Assertions
  expect(global.fetch).toHaveBeenCalledTimes(1);
  // Add more assertions as needed
});

test('Form submission fetch failure does not show success message', async () => {
  // Mock fetch to simulate a failure response
  global.fetch.mockImplementationOnce(() =>
    Promise.resolve({
      ok: false,
      statusText: 'Internal Server Error',
    })
  );

  document.getElementById('title').value = 'Valid Title';
  document.getElementById('description').value = 'Valid Description';
  document.getElementById('author').value = 'Valid Author';
  document.getElementById('adminKey').value = 'Valid Key';

  document.getElementById('createAnnouncementForm').dispatchEvent(new Event('submit'));

  await new Promise(process.nextTick); // Wait for any asynchronous operations to complete

});



