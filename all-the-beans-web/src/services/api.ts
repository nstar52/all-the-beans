import axios from 'axios';
import type { Bean } from '../types/Bean';

const API_BASE_URL = 'http://localhost:5114/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const getAllBeans = async (): Promise<Bean[]> => {
  const response = await api.get<Bean[]>('/beans');
  return response.data;
};

export const getBeanById = async (id: number): Promise<Bean> => {
  const response = await api.get<Bean>(`/beans/${id}`);
  return response.data;
};

export const getBeanOfTheDay = async (): Promise<Bean> => {
  const response = await api.get<Bean>('/beanoftheday');
  return response.data;
};

export const searchBeans = async (
  name?: string,
  country?: string,
  colour?: string
): Promise<Bean[]> => {
  const params = new URLSearchParams();
  if (name) params.append('name', name);
  if (country) params.append('country', country);
  if (colour) params.append('colour', colour);

  const queryString = params.toString();
  const response = await api.get<Bean[]>(`/beans/search?${queryString}`);
  return response.data;
};

