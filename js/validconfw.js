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
        this.errors.push("Name required Vue.");
      }
      if (!this.email) {
        this.errors.push("Email required Vue.");
      } else if (!this.validEmail(this.email)) {
        this.errors.push("Valid email required Vue.");
      }

      return !this.errors.length;
    },
    validEmail(email) {
      const re =
        /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(email);
    },
    handleSubmit() {
      if (this.checkForm()) {
        // Form is valid, submit it
        this.$refs.form.submit();
      }
    },
    handleButtonClick() {
      if (this.checkForm()) {
        // Form is valid, submit it
        this.$refs.form.submit();
      } else {
        alert("Form is invalid from Vue ");
      }
    },
  },
}).mount("#app");