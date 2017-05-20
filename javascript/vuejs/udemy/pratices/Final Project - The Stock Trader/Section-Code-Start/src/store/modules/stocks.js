import stocks from '../../data/stocks.js'

const state = {
    stocks: []
};

const mutations = {
    'SET_STOCKS'(state, stocks) {
        state.stocks = stocks;
    },
    'RND_STOCK'(state) {

    }
};

const actions = {
    buyStocks: ({ commit }, order) => {
        commit('BUY_STOCKS', order);
    },
    initStocks: ({ commit }) => {
        commit('SET_STOCKS', stocks);
    },
    randomizeStocks: ({ commit }) => {
        commit('RND_STOCKs');
    }
};

const getters = {
    stocks: state => {
        return state.stocks;
    }
}

export default {
    state,
    mutations,
    actions,
    getters
};