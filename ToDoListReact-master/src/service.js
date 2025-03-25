import axios from 'axios';
axios.defaults.baseURL = process.env.REACT_APP_API_URL
axios.interceptors.response.use(
  response => response, 
  error => {
    console.error('Axios error:', error.response ? error.response.data : error.message);
    return Promise.reject(error); 
  }
);
export default {
  // שליפה
  getTasks: async () => {
    const result = await axios.get(`/getAll`)
    return result.data;
  },
  // הוספה
  addTask: async (name) => {
    const result = await axios.post(`/addTask?s=${encodeURIComponent(name)}`);
    console.log('addTask', name);
    return result.data;
  },
  // עדכון
  setCompleted: async (id) => {
    console.log('setCompleted', id)
    const result = await axios.patch(`/updateTask/${id}`)
    return result.data;
  },
 // מחיקה
  deleteTask: async (id) => {
    const result = await axios.delete(`/delete/${id}`)
    console.log('deleteTask')
    return result.data;
  }
};
