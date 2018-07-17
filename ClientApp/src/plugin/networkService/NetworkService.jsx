import axios from 'axios';

//let numberOfAjaxCAllPending = 0;

export default {
    setupRequestInterceptors: () => {
        axios.interceptors.request.use(function (config) {
            config.headers.authorization = "bearer " + localStorage.getItem("token");
            //numberOfAjaxCAllPending++;
            //console.log("show");
            return config;
        }, function (error) {
            return Promise.reject(error);
        });
    },
    setupResponseInterceptors: () => {
        axios.interceptors.response.use(function (config) {
            //numberOfAjaxCAllPending--;
            //if (numberOfAjaxCAllPending === 0) {
            //    //hide loader
            //    console.log("hide");
            //}
            return config;
        }, function (error) {
            if (error.response !== undefined) {
                let erroResponse = error.response;

                if (erroResponse.status === 401) {
                    return axios.post('api/Token/RefreshToken', {
                        UserId: localStorage.getItem("user"),
                        Refresh: localStorage.getItem("refresh")
                    }).then(response => {
                        let data = JSON.parse(response.data);

                        if (data.Status === "SUCCESS") {
                            var result = JSON.parse(data.Result);
                            localStorage.setItem("token", result.Token);
                            localStorage.setItem("refresh", result.Refresh);
                            localStorage.setItem("user", result.UserId);
                        }
                        return axios(erroResponse.config);
                    }).catch(error => {
                        return Promise.reject(error);
                    });
                }
            }
            return Promise.reject(error);
        });
    }

}