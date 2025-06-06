import { request }  from './api.js';
import { getToken } from './auth.js';

const list     = document.getElementById('recipeList');
const alertBox = document.getElementById('recipeAlert');
const btnNew   = document.getElementById('btnNewRecipe');

const modalEl  = document.getElementById('recipeModal');
const modal    = new bootstrap.Modal(modalEl);
const form     = document.getElementById('recipeForm');

let currentUser=null;

async function fetchMe(){ return getToken()? request('/users/me'):null; }

function card(r){
  const mine = currentUser && r.author_id===currentUser.id;
  return `
  <div class="col-sm-6 col-lg-4" id="card-${r.id}">
    <div class="card h-100 shadow-sm">
      <div class="card-body d-flex flex-column">
        <h5>${r.title}</h5>
        <p class="flex-grow-1">${r.description||''}</p>
        ${mine?`
          <div class="d-flex gap-2">
            <button class="btn btn-outline-secondary btn-sm flex-fill" data-edit="${r.id}">Изменить</button>
            <button class="btn btn-outline-danger btn-sm flex-fill"    data-del="${r.id}">Удалить</button>
          </div>`:`
            <button class="btn btn-outline-primary mt-2" data-view="${r.id}">Смотреть</button>`}
      </div>
    </div>
  </div>`;
}

async function load(){
  try{
    currentUser = await fetchMe();
    if(getToken()) btnNew.classList.remove('d-none');

    const recipes = await request('/recipes');
    list.innerHTML = recipes.map(card).join('');

  }catch(err){ alertBox.textContent=err.message; alertBox.classList.remove('d-none'); }
}
load();

const viewTitle = document.getElementById('viewTitle');
const viewInstr = document.getElementById('viewInstr');
const viewIngr  = document.getElementById('viewIngr');
const viewModal = new bootstrap.Modal('#viewModal');

list.addEventListener('click', async e=>{
  const id = e.target.dataset.edit || e.target.dataset.del || e.target.dataset.view;
  if(!id) return;

  try{
      if (e.target.dataset.edit) {
        const r = await request(`/recipes/${id}`);

        let inst = r.instructions || '';

        if (inst.length >= 2 && inst[0] === '"' && inst[inst.length - 1] === '"') {
          inst = inst.slice(1, -1);
        }

        let ingr = r.ingredients || [];
        if (Array.isArray(ingr)) {
          // превращаем ['Тест 1','Тест 2','Тест 3'] в 'Тест 1\nТест 2\nТест 3'
          ingr = ingr.join('\n');
        } else if (typeof ingr === 'string') {

          if (ingr.startsWith('{') && ingr.endsWith('}')) {
            const arr = ingr.slice(1, -1).split(',');
            ingr = arr.join('\n');
          }
        
        }

        if (form.title) {
          form.title.value = r.title || '';
        }
        if (form.description) {
          form.description.value = r.description || '';
        }
        if (form.instructions) {
          form.instructions.value = inst;
        }
        if (form.ingredients) {
          form.ingredients.value = ingr;
        }
        // Если есть другие поля (например, id), их можно заполнить:
        if (form.id) {
          form.id.value = r.id;
        }

        modal.show();
      }
      else if(e.target.dataset.del){
        if(confirm('Удалить рецепт?')){
          await request(`/recipes/${id}`,{ method:'DELETE' });
          document.getElementById(`card-${id}`).remove();
        }
      }
      else if(e.target.dataset.view){
        const r = await request(`/recipes/${id}`);
        viewTitle.textContent = r.title;
        viewInstr.textContent = viewInstr.textContent = r.instructions || '';

        viewIngr.innerHTML = (r.ingredients || [])
          .map(i => `<li>${i}</li>`)
          .join('');

        viewModal.show();
      }
  }
  catch(err){ 
    alert(err.message); }
});

btnNew?.addEventListener('click',()=>{ form.reset(); form.id.value=''; modal.show(); });

form.addEventListener('submit', async e=>{
  e.preventDefault();
  const data = Object.fromEntries(new FormData(form));
  data.ingredients = data.ingredients.split('\n').filter(Boolean);
  try{
    if(data.id)
      await request(`/recipes/${data.id}`, { method:'PUT', body:JSON.stringify(data) });
    else
      await request('/recipes', { method:'POST', body:JSON.stringify(data) });

    modal.hide(); load();
  }catch(err){ alert(err.message); }
});
