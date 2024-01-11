export type UserTokens = {
  accessToken: string;
  refreshToken: string;
};

class TokenService {
  getLocalRefreshToken() {
    const userTokens: UserTokens = JSON.parse(
      localStorage.getItem("userTokens")!
    );
    return userTokens?.refreshToken;
  }

  getLocalAccessToken() {
    const userTokens: UserTokens = JSON.parse(
      localStorage.getItem("userTokens")!
    );
    return userTokens?.accessToken;
  }

  updateLocalAccessToken(token: string) {
    let userTokens: UserTokens = JSON.parse(
      localStorage.getItem("userTokens")!
    );
    userTokens.accessToken = token;
    localStorage.setItem("userTokens", JSON.stringify(userTokens));
  }

  getUserTokens() {
    return JSON.parse(localStorage.getItem("userTokens")!);
  }

  isUserLogged() {
    return this.getUserTokens() !== null ? true : false;
  }

  setUserTokens(userTokens: UserTokens) {
    localStorage.setItem("userTokens", JSON.stringify(userTokens));
  }

  removeUserTokens() {
    localStorage.removeItem("userTokens");
  }
}

export default new TokenService();
