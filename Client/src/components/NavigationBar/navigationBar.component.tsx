import { NavLink, Outlet, useNavigate } from "react-router-dom";
import jwt_decode from "jwt-decode";
import { AppBar, Box, Button, Toolbar } from "@mui/material";
import "./navigationBar.component.style.css";
import AuthService from "../../Services/auth.service";
import TokenService from "../../Services/token.service";

export default function Root() {
  const navigate = useNavigate();
  let token = TokenService.getLocalAccessToken();
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
          </div>
          <div className="name-info">
            {token && `${tokenClaims?.name} ${tokenClaims?.family_name}`}
          </div>
          <NavLink
            className="nav-link"
            onClick={() => {
              AuthService.logout();
            }}
            to={"/login"}
          >
            Log out
          </NavLink>
        </Toolbar>
      </div>
      <Outlet />
    </div>
  );
}
