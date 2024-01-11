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

  //   GetTest(testId: number) {
  //     return api.get(`${get_tests_route}?testId=${testId}`).then((response) => {
  //       if (response.data.accessToken) {
  //         TokenService.setUserTokens({
  //           accessToken: response.data.accessToken,
  //           refreshToken: response.data.refreshToken,
  //         });
  //       }

  //       return response.data;
  //     });
  //   }

  //   GetTestQuestionsWithAnswers(testId: number) {
  //     return api
  //       .get(`${get_test_questions_with_answers_route}?testId=${testId}`)
  //       .then((response) => {
  //         if (response.data.accessToken) {
  //           TokenService.setUserTokens({
  //             accessToken: response.data.accessToken,
  //             refreshToken: response.data.refreshToken,
  //           });
  //         }

  //         return response.data;
  //       });
  //   }

  //   GetAllUserPassedTestIds() {
  //     return api.get(`${user_completed_test_ids_route}`).then((response) => {
  //       if (response.data.accessToken) {
  //         TokenService.setUserTokens({
  //           accessToken: response.data.accessToken,
  //           refreshToken: response.data.refreshToken,
  //         });
  //       }

  //       return response.data;
  //     });
  //   }
}

export default new UrlService();
