import {ActionTree} from 'vuex';
import axios from 'axios';
import {UserState} from './types';
import {RootState} from '../types';
import {User} from "@/models/User";
import {Token} from "@/models/Token";

export const actions: ActionTree<UserState, RootState> = {
    fetchJWT({commit}, {username, password}): any {
        // Perform the HTTP request.
        //let token = new Token('');
        axios
            .put<Token>('api/Users/RequestToken/' + username,
                password,
                {headers: {"Content-Type": "application/json"}}
            )
            .then((response) => {
                commit('setJWT', response.data.token);
            });
    },
};
