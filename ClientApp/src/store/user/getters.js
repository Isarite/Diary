export const getters = {
    currentToken(state) {
        return state.currentJWT;
    },
    jwtData(state, getters) {
        return state.currentJWT ?
            JSON.parse(atob(getters.jwt.split('.')[1])) : null;
    },
    jwtSubject(state, getters) {
        return getters.jwtData ? getters.jwtData.sub : null;
    },
    jwtIssuer: (state, getters) => getters.jwtData ? getters.jwtData.iss : null
};
//# sourceMappingURL=getters.js.map