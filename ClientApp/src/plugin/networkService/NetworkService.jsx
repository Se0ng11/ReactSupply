import axios from 'axios';

export default {
    setupInterceptors: () => {
        axios.interceptors.request.use(function (config) {
            return config;
        }, function (error) {
            let erroResponse = error.response;

            if (erroResponse.status === 401) {
                window.axios.interceptors.response.eject(this.setupInterceptors);
                return window.axios.post('/api/Auth/RefreshToken', {
                    Refresh: localStorage.getItem("refresh")
                }).then(response => {
                    let data = JSON.parse(response);

                    if (data.Status === "SUCCESS") {
                        localStorage.setItem("token", response.Token);
                    }

                    erroResponse.config.headers['Authorization'] = 'Bearer ' + response.data.access_token;
                    this.createAxiosResponseInterceptor();
                    return window.axios(erroResponse.config);
                }).catch(error => {
                    //this.destroyToken();
                    //this.createAxiosResponseInterceptor();
                    //this.router.push('/login');
                    return Promise.reject(error);
                });
            }
            return Promise.reject(error);
        });
    }

}