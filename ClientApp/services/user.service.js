import authHeader from "../helpers";
import axios from 'axios'


export const userService = {
  login,
  registry,
  logout,
  getAll
};

function login(username, password) {


    return axios({
    method: 'post',
    url: 'auth/user/authenticate',
    data: { username, password },
    config: {
      headers: {
        'Accept': "application/json",
        'Content-Type': "application/json"
      }
    }
  }).then(handleResponse)
    .then(user => {

      if (user.body.token) {

        localStorage.setItem("user", JSON.stringify(user.body));
      }

      return user.body;
    });
}

function registry(user) {
  return axios({
    method: "post",
    url: "auth/user/registry",
    data: user
    
    
  }).then(handleResponse)
    .then(u => {
       return login(u.body.username, u.body.password);
    });
}

function logout() {
  localStorage.removeItem("user");
}

function getAll() {
  return axios({ method: "post", url: `auth/user/getall`, config: { headers: authHeader() }}).then(handleResponse);
}


function handleResponse(response) {


  if (!(response.status === 200)) {
    if (response.status === 401) {
      logout();
      location.reload(true);
    }

    const error = response.data.message || response.statusText;
    return Promise.reject(error);
  }

  return response.data;

}
