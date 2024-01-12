import { NavLink, Outlet, useNavigate } from "react-router-dom";
import jwt_decode from "jwt-decode";
import { AppBar, Box, Button, Toolbar } from "@mui/material";
import "./navigationBar.component.style.css";
import AuthService from "../../Services/auth.service";
import tokenService from "../../Services/token.service";

export default function Root() {
  const userIsLogged = tokenService.isUserLogged();
  let token = tokenService.getLocalAccessToken();
  const tokenClaims = token
    ? jwt_decode<{ name: string; family_name: string }>(`${token}`)
    : undefined;

  return (
    <div className="content-wrap">
      <div className="nav-bar-wrap">
        <Toolbar>
          <div className="nav-buttons">
            <NavLink className="nav-link" to={"/"}>
              Home
            </NavLink>
            <NavLink className="nav-link" to={"/about"}>
              About
            </NavLink>
          </div>
          <div className="name-info">
            {token && `${tokenClaims?.name} ${tokenClaims?.family_name}`}
          </div>
          {userIsLogged ? (
            <NavLink
              className="nav-link"
              onClick={() => {
                AuthService.logout();
              }}
              to={"/login"}
            >
              Log out
            </NavLink>
          ) : (
            <>
              {" "}
              <NavLink className="nav-link" to={"/login"}>
                Log In
              </NavLink>
              <NavLink className="nav-link" to={"/register"}>
                Register
              </NavLink>
            </>
          )}
        </Toolbar>
      </div>
      <Outlet />
    </div>
  );
}
