// Shows the server's error messages under a form field (nothing if there are none).
function FieldError({ messages }) {
  if (!messages) return null;
  return <span className="field-error">{messages.join(" ")}</span>;
}

export default FieldError;
