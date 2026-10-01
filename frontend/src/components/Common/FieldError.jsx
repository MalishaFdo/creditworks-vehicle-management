// Show validation errors under a form field.
function FieldError({ messages }) {
  if (!messages) return null;
  return <span className="field-error">{messages.join(" ")}</span>;
}

export default FieldError;
