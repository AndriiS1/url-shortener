import api from "./api";
import TokenService from "./token.service";
import { url_route } from "../ApiRoutes/apiRoutes";

class UrlService {
  GetTableUrlsData() {
    return api.get(url_route).then((response) => response.data);
  }

  CreateShortUlr(originalUrl: string) {
    return api
      .post(url_route, { originalUrl })
      .then((response) => response.data);
  }

  GetUrlInfo(id: number) {
    return api.get(`${url_route}/${id}`).then((response) => response.data);
  }

  DeleteUrl(id: number) {
    return api.delete(`${url_route}/${id}`).then((response) => response.data);
  }
}

export default new UrlService();
