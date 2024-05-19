const { createApp } = Vue;

createApp({
  data() {
    return {
      name: "",
      email: "",
      errors: [],
    };
  },
  methods: {
    checkForm() {
      this.errors = [];

      if (!this.name) {
        this.errors.push("Name required.");
      }
      if (!this.email) {
        this.errors.push("Email required.");
      } else if (!this.validEmail(this.email)) {
        this.errors.push("Valid email required.");
      }

      return !this.errors.length;
    },
    validEmail(email) {
      const re =
        /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(email);
    },
    handleSubmit(e) {
      if (this.checkForm()) {
        // Form is valid, you can submit it or perform any action you need.
        alert("Form is valid");
      } else {
        e.preventDefault();
      }
    },
    handleButtonClick() {
      if (this.checkForm()) {
        // Form is valid, you can submit it or perform any action you need.
        alert("Form is valid");
      } else {
        alert("Form is invalid");
      }
    },
  },
}).mount("#app");
