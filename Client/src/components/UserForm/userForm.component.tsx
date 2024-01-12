import { useEffect, useState } from "react";
import "./userForm.component.style.css";
import { Form, Link, useNavigate } from "react-router-dom";
import { Button, FormControl } from "@mui/material";
import { Snackbar, TextField } from "@mui/material";
import { AxiosError } from "axios";
import AuthService from "../../Services/auth.service";
import TokenService from "../../Services/token.service";

export enum userFormType {
  login,
  register,
}

export default function UserForm(props: { formType: userFormType }) {
  const navigate = useNavigate();
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [firstName, setFirstName] = useState<string>("");
  const [secondName, setSecondName] = useState<string>("");
  const [open, setOpen] = useState<boolean>(false);
  const [axiosErrorMessage, setAxiosErrorMessage] = useState<any>("");

  const [emailError, setEmailError] = useState<boolean>(false);
  const [passwordError, setPasswordError] = useState<boolean>(false);
  const [firstNameError, setFirstNameError] = useState<boolean>(false);
  const [secondNameError, setSecondNameError] = useState<boolean>(false);

  const nameRegexPatter = new RegExp("^[a-zA-Z0-9]*$");
  const emailRegexPatter = new RegExp(
    "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$"
  );
  const passwordRegexPatter = new RegExp(
    "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]{8,}$"
  );

  const userLoginDataIsValid = !emailError && !passwordError;
  const userRegisterDataIsValid =
    !emailError && !passwordError && !firstNameError && !secondNameError;

  enum InputType {
    email,
    password,
    firstName,
    secondName,
  }

  useEffect(() => {
    const validateField = (
      field: string,
      regexPattern: RegExp,
      setError: (error: boolean) => void
    ) => {
      field && setError(!regexPattern.test(field));
    };

    validateField(firstName, nameRegexPatter, setFirstNameError);
    validateField(secondName, nameRegexPatter, setSecondNameError);
    validateField(email, emailRegexPatter, setEmailError);
    validateField(password, passwordRegexPatter, setPasswordError);
  }, [firstName, secondName, email, password]);

  enum SubmitType {
    login,
    register,
  }

  const HandleSubmit = async (submitType: SubmitType) => {
    try {
      if (userLoginDataIsValid && submitType === SubmitType.login) {
        await AuthService.login(email, password);
      } else if (
        userRegisterDataIsValid &&
        submitType === SubmitType.register
      ) {
        await AuthService.register({
          firstName,
          secondName,
          email,
          password,
        });
      }
      if (TokenService.getUserTokens()) {
        navigate("/");
      }
    } catch (e) {
      const error = e as AxiosError;
      setAxiosErrorMessage(error?.response?.data || error?.message);
      console.log(error);
      setOpen(true);
    }
  };

  return (
    <div className="form-wrap">
      <Form
        className="form-container"
        onSubmit={
          props.formType === userFormType.login
            ? () => HandleSubmit(SubmitType.login)
            : () => HandleSubmit(SubmitType.register)
        }
      >
        <span className="form-title">
          {props.formType === userFormType.login ? "Login" : "Register"}
        </span>
        {props.formType == userFormType.register && (
          <>
            <TextField
              error={firstNameError}
              onChange={(e) => setFirstName(e.target.value)}
              required
              placeholder="Name"
              margin="dense"
              size="small"
              label="First name"
            />
            <TextField
              error={secondNameError}
              onChange={(e) => setSecondName(e.target.value)}
              required
              placeholder="Surname"
              margin="dense"
              size="small"
              label="Second name"
            />
          </>
        )}
        <TextField
          error={emailError}
          onChange={(e) => setEmail(e.target.value)}
          required
          placeholder="example@gmail.com"
          margin="dense"
          size="small"
          label="Email"
        />
        <TextField
          error={passwordError}
          onChange={(e) => setPassword(e.target.value)}
          required
          type="password"
          placeholder="Password"
          margin="dense"
          size="small"
          label="Password"
        />
        <Button className="form-element" type="submit">
          Submit
        </Button>
        <Link
          className="register-link form-element"
          to={props.formType === userFormType.login ? "/register" : "/login"}
        >
          {props.formType === userFormType.login ? "Register" : "Log in"}
        </Link>
        <Snackbar
          open={open}
          onClose={() => setOpen(false)}
          autoHideDuration={4000}
          message={axiosErrorMessage}
        />
      </Form>
    </div>
  );
}
