export type UserTokens = {
  accessToken: string;
  refreshToken: string;
};

class TokenService {
  getLocalRefreshToken() {
    const user = JSON.parse(localStorage.getItem("userTokens")!);

    return user?.refreshToken;
  }

  getLocalAccessToken() {
    const userTokens = JSON.parse(localStorage.getItem("userTokens")!);
    return userTokens?.accessToken;
  }

  updateLocalAccessToken(token: string) {
    let userTokens = JSON.parse(localStorage.getItem("userTokens")!);
    userTokens.accessToken = token;
    localStorage.setItem("userTokens", JSON.stringify(userTokens));
  }

  getUserTokens() {
    return JSON.parse(localStorage.getItem("userTokens")!);
  }

  setUserTokens(userTokens: UserTokens) {
    console.log(JSON.stringify(userTokens));
    localStorage.setItem("userTokens", JSON.stringify(userTokens));
  }

  removeUserTokens() {
    localStorage.removeItem("userTokens");
  }
}

export default new TokenService();
