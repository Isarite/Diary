import {MutationTree} from 'vuex';
import {UserState} from './types';

export const mutations: MutationTree<UserState> = {
    setJWT(state, jwt) {
        // When this updates, the getters and anything bound to them updates as well.
        state.currentJWT = jwt;
    }
};
