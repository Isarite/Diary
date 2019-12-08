import { __decorate } from "tslib";
import { Component, Vue } from 'vue-property-decorator';
import axios from 'axios';
let FetchDataView = class FetchDataView extends Vue {
    constructor() {
        super(...arguments);
        this.loading = true;
        this.showError = false;
        this.errorMessage = 'Error while loading weather forecast.';
        this.forecasts = [];
        this.headers = [
            { text: 'Date', value: 'dateFormatted' },
            { text: 'Temp. (C)', value: 'temperatureC' },
            { text: 'Temp. (F)', value: 'temperatureF' },
            { text: 'Summary', value: 'summary' },
        ];
    }
    created() {
        this.fetchWeatherForecasts();
    }
    fetchWeatherForecasts() {
        axios
            .get('api/SampleData/WeatherForecasts')
            .then((response) => {
            this.forecasts = response.data;
        })
            .catch((e) => {
            this.showError = true;
            this.errorMessage = `Error while loading weather forecast: ${e.message}.`;
        })
            .finally(() => (this.loading = false));
    }
};
FetchDataView = __decorate([
    Component({})
], FetchDataView);
export default FetchDataView;
//# sourceMappingURL=FetchData.vue.js.map