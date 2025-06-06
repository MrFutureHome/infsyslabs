import { request }  from './api.js';
import { getToken } from './auth.js';

const list      = document.getElementById('classList');
const alertBox  = document.getElementById('classAlert');
const btnNew    = document.getElementById('btnNewClass');

const modalEl   = document.getElementById('classModal');
const modal     = new bootstrap.Modal(modalEl);
const form      = document.getElementById('classForm');

let currentUser = null;

async function fetchMe(){
  if(!getToken()) return null;
  return request('/users/me');
}
function cardHtml(c){
  const mine = currentUser && c.author_id === currentUser.id;
  return `
  <div class="col-md-4" id="card-${c.id}">
    <div class="card h-100 shadow-sm">
      <video class="card-img-top" src="${c.video_url||''}" controls muted></video>
      <div class="card-body d-flex flex-column">
        <h5>${c.title}</h5>
        <p class="text-muted small mb-2">${c.chef||''}</p>
        <p class="flex-grow-1">${c.description||''}</p>
        ${mine ? `
          <div class="d-flex gap-2">
            <button class="btn btn-outline-secondary btn-sm flex-fill" data-edit="${c.id}">Изменить</button>
            <button class="btn btn-outline-danger btn-sm flex-fill"    data-del="${c.id}">Удалить</button>
          </div>` :
          getToken()? `<button class="btn btn-primary" data-reg="${c.id}">Записаться</button>` : ''}
      </div>
    </div>
  </div>`;
}

async function load(){
  try{
    currentUser = await fetchMe();
    if(getToken()) btnNew.classList.remove('d-none');

    const classes = await request('/classes');
    list.innerHTML = classes.map(cardHtml).join('');
  }catch(err){
    alertBox.textContent=err.message; alertBox.classList.remove('d-none');
  }
}
load();

btnNew?.addEventListener('click',()=>{
  form.reset(); form.id.value=''; modal.show();
});

// отправка формы
form.addEventListener('submit', async e=>{
  e.preventDefault();
  const data = Object.fromEntries(new FormData(form));
  try{
    if(data.id){
      await request(`/classes/${data.id}`, { method:'PUT', body:JSON.stringify(data) });
    }else{
      await request('/classes', { method:'POST', body:JSON.stringify(data) });
    }
    modal.hide(); load();
  }catch(err){ alert(err.message); }
});

// клики по карточкам
list.addEventListener('click', async e=>{
  const id = e.target.dataset.edit || e.target.dataset.del || e.target.dataset.reg;
  if(!id) return;

  try{
    if(e.target.dataset.edit){
      const cls = await request(`/classes/${id}`);
      Object.entries(cls).forEach(([k,v])=> form[k]!==undefined && (form[k].value=v));
      modal.show();

    }else if(e.target.dataset.del){
      if(confirm('Удалить?')){
        await request(`/classes/${id}`,{ method:'DELETE' });
        document.getElementById(`card-${id}`).remove();
      }

    }else if(e.target.dataset.reg){
      await request(`/registrations/${id}`,{ method:'POST' });
      e.target.textContent='Вы записаны'; e.target.disabled=true;
    }
  }catch(err){ alert(err.message); }
});
