function ValidateEmail(inputText) {
  var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
  if(inputText.value.match(mailformat)) {
    return true;
  } else {
    alert("You have entered an invalid email address(javascript)!");
    return false;
  }
}
function required() {
  var empt = document.form.name.value;
  if (empt === "") {
    alert("Please input a Value(javascript)");
    return false;
  } else {
    return true;
  }
}
function validateForm() {
  var emailValid = ValidateEmail(document.form.email);
  var requiredValid = required();
  return emailValid && requiredValid;
}
function handleClick() {
  var emailValid = ValidateEmail(document.form.email);
  var requiredValid = required();
  if (emailValid && requiredValid) {
    document.form.submit();
  }
}