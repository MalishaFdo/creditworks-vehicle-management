// Get validation errors for each field.
export function getFieldErrors(error) {
  return error.response?.data?.errors || {};
}

// Get a general error message.
export function getErrorMessage(error) {
  if (!error.response) {
    return "Cannot reach the server.";
  }

  const data = error.response.data;
  return data?.detail || data?.title || "Something went wrong. Please try again.";
}
