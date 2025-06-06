// хранение / проверка JWT, динамика навбара
import { request } from './api.js';

const TOKEN_KEY = 'token';

export function setToken(t){ localStorage.setItem(TOKEN_KEY,t);}
export function getToken(){ return localStorage.getItem(TOKEN_KEY);}
export function clearToken(){ localStorage.removeItem(TOKEN_KEY);}

export async function login(email, password){
  const { token } = await request('/users/login',{
    method:'POST',
    body: JSON.stringify({email,password})
  });
  setToken(token);
}

export async function register(email, password){
  const { token } = await request('/users/register',{
    method:'POST',
    body: JSON.stringify({email,password})
  });
  setToken(token);
}

export function logout(){ clearToken(); }

// Показывать правильные пункты меню
export function updateNav(){
  const logged = !!getToken();
  document.querySelectorAll('#navProfile,#navLogout').forEach(el=>el?.classList.toggle('d-none',!logged));
  document.querySelectorAll('#navLogin').forEach(el=>el?.classList.toggle('d-none',logged));
}

// Перенаправить на /login.html, если не авторизован
export function ensureAuth(){
  if(!getToken()) location.href='/login.html';
}

updateNav();
window.addEventListener('storage', updateNav);  // реакция, если в другой вкладке вышли
