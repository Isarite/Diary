import axios from 'axios';
export const actions = {
    fetchJWT({ commit }, { username, password }) {
        // Perform the HTTP request.
        //let token = new Token('');
        axios
            .put('api/Users/RequestToken/' + username, password, { headers: { "Content-Type": "application/json" } })
            .then((response) => {
            commit('setJWT', response.data.token);
        });
    },
};
//# sourceMappingURL=actions.js.map