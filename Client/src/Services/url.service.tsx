import api from "./api";
import TokenService from "./token.service";
import { url_route } from "../ApiRoutes/apiRoutes";

class UrlService {
  GetTableUrlsData() {
    return api.get(url_route).then((response) => {
      return response.data;
    });
  }

  CreateShortUlr(originalUrl: string) {
    console.log(originalUrl);
    return api.post(url_route, { originalUrl }).then((response) => {
      if (response.data.accessToken) {
        TokenService.setUserTokens({
          accessToken: response.data.accessToken,
          refreshToken: response.data.refreshToken,
        });
      }

      return response.data;
    });
  }

  GetUrlInfo(id: number) {
    return api.get(`${url_route}/${id}`).then((response) => {
      return response.data;
    });
  }

  DeleteUrl(id: number) {
    return api.delete(`${url_route}/${id}`).then((response) => {
      return response.data;
    });
  }
}

export default new UrlService();
