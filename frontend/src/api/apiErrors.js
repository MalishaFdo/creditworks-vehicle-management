// Turns an error from axios into simple messages we can show on the page.

// Errors for each field, e.g. { weightKg: ["Weight must be greater than 0 kg."] }
export function getFieldErrors(error) {
  return error.response?.data?.errors || {};
}

// One message for the whole request.
export function getErrorMessage(error) {
  if (!error.response) {
    return "Cannot reach the server. Check that the API is running.";
  }

  const data = error.response.data;
  return data?.detail || data?.title || "Something went wrong. Please try again.";
}
