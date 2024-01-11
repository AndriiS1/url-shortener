import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import Root from "./components/Main/main.component";
import ErrorPage from "./components/ErrorPage/errorPage.component";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import UserForm, {
  userFormType,
} from "./components/UserForm/userForm.component";
import ProtectedWrap from "./components/ProtectedWrap/protectedWrap.component";
import TestsList from "./components/ShortenUrlsTable/shortenUrlsTable.component";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <ProtectedWrap>
        <Root />
      </ProtectedWrap>
    ),
    // children: [
    //   {
    //     path: "/",
    //     element: <TestsList></TestsList>,
    //   },
    //   {
    //     path: "test",
    //     element: <TestsPage></TestsPage>,
    //   },
    // ],
    errorElement: <ErrorPage />,
  },
  {
    path: "/login",
    element: <UserForm formType={userFormType.login} />,
  },
  {
    path: "register",
    element: <UserForm formType={userFormType.register} />,
  },
]);

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);
root.render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);
