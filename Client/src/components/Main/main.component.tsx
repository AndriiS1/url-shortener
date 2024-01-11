import { Outlet, useNavigate } from "react-router-dom";
import jwt_decode from "jwt-decode";
import { AppBar, Box, Button, Toolbar } from "@mui/material";
import "./main.component.style.css";
import AuthService from "../../Services/auth.service";
import TokenService from "../../Services/token.service";

export default function Root() {
  const navigate = useNavigate();
  let token = TokenService.getLocalAccessToken();
  const tokenClaims = token
    ? jwt_decode<{ name: string; family_name: string }>(`${token}`)
    : undefined;

  return (
    <div className="menu-container">
      <Box sx={{ flexGrow: 1 }}>
        <AppBar position="static">
          <Toolbar>
            <Box sx={{ flexGrow: 1, display: { xs: "none", md: "flex" } }}>
              <Button
                sx={{ color: "white", display: "block", boxShadow: "none" }}
                onClick={() => navigate("/")}
              >
                Home
              </Button>
            </Box>
            <div className="name-info">
              {token && `${tokenClaims?.name} ${tokenClaims?.family_name}`}
            </div>
            <Button
              sx={{ color: "white", display: "block", boxShadow: "none" }}
              onClick={() => {
                AuthService.logout();
                navigate("/login");
              }}
            >
              Log out
            </Button>
          </Toolbar>
        </AppBar>
        <Outlet />
      </Box>
    </div>
  );
}
