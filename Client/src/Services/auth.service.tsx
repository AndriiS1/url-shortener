import api from "./api";
import TokenService from "./token.service";
import {
  registration_route,
  authentication_route,
} from "../ApiRoutes/apiRoutes";

type RegisterModel = {
  firstName: string;
  secondName: string;
  email: string;
  password: string;
};

class AuthService {
  login(email: string, password: string) {
    return api
      .post(authentication_route, {
        email,
        password,
      })
      .then((response) => {
        if (response.data.accessToken) {
          TokenService.setUserTokens({
            accessToken: response.data.accessToken,
            refreshToken: response.data.refreshToken,
          });
        }

        return response.data;
      });
  }

  logout() {
    TokenService.removeUserTokens();
  }

  register(registerData: RegisterModel) {
    return api
      .post(registration_route, {
        firstName: registerData.firstName,
        secondName: registerData.secondName,
        email: registerData.email,
        password: registerData.password,
      })
      .then((response) => {
        if (response.data.accessToken) {
          TokenService.setUserTokens({
            accessToken: response.data.accessToken,
            refreshToken: response.data.refreshToken,
          });
        }

        return response.data;
      });
  }

  getCurrentUserTokens() {
    return TokenService.getUserTokens();
  }
}

export default new AuthService();
