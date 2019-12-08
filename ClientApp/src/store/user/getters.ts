import {GetterTree} from 'vuex';
import {UserState} from './types';
import {RootState} from '../types';

export const getters: GetterTree<UserState, RootState> = {
    currentToken(state): string {
        return state.currentJWT;
    },
    jwtData(state, getters): string {
        return state.currentJWT ?
            JSON.parse(
                atob(getters.jwt.split('.')[1])) : null;
    },
    jwtSubject(state, getters): string {
        return getters.jwtData ? getters.jwtData.sub : null;
    },
    jwtIssuer: (state, getters) => getters.jwtData ? getters.jwtData.iss : null
};
