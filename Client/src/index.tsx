import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import Root from "./components/NavigationBar/navigationBar.component";
import ErrorPage from "./components/ErrorPage/errorPage.component";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import UserForm, {
  userFormType,
} from "./components/UserForm/userForm.component";
import ProtectedWrap from "./components/ProtectedWrap/protectedWrap.component";
import UrlsTable from "./components/UrlsTable/urlsTable.component";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
    children: [
      {
        path: "/",
        element: <UrlsTable />,
      },
    ],
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
