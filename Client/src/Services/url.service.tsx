import api from "./api";
import TokenService from "./token.service";
import { get_table_urls_data } from "../ApiRoutes/apiRoutes";

class UrlService {
  GetTableUrlsData() {
    return api.get(get_table_urls_data).then((response) => {
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

  //   PassTest(testId: number, mark: number) {
  //     return api
  //       .post(`${test_passed_route}`, { testId, mark })
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
}

export default new UrlService();
