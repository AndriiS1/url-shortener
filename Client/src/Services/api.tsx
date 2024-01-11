import axios from "axios";
import TokenService from "./token.service";
import {
  server_url,
  authentication_route,
  refresh_route,
} from "../ApiRoutes/apiRoutes";

const api = axios.create({
  baseURL: server_url,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.request.use(
  (config) => {
    const token = TokenService.getLocalAccessToken();
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (res) => {
    return res;
  },
  async (err) => {
    const originalConfig = err.config;

    if (originalConfig.url !== authentication_route && err.response) {
      if (err.response.status === 401 && !originalConfig._retry) {
        originalConfig._retry = true;

        try {
          const response = await api.post(refresh_route, {
            refreshToken: TokenService.getLocalRefreshToken(),
          });

          const { accessToken } = response.data;
          TokenService.updateLocalAccessToken(accessToken);

          return api(originalConfig);
        } catch (_error) {
          return Promise.reject(_error);
        }
      }
    }

    return Promise.reject(err);
  }
);

export default api;
