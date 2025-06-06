import { request } from './api.js';

document.addEventListener('DOMContentLoaded', async () => {
  try {
    const classes = await request('/recommendations/for-you');
    const container = document.getElementById('recommendations');

    classes.forEach(c => {
      container.insertAdjacentHTML('beforeend', `
        <div class="col-md-4 mb-4">
          <div class="card h-100 shadow">
            <video class="card-img-top" src="${c.video_url}" controls muted></video>
            <div class="card-body">
              <h5 class="card-title">${c.title}</h5>
              <p class="card-text">${c.description}</p>
              <a href="/masterclasses.html#${c.id}" class="btn btn-outline-primary">Подробнее</a>
            </div>
          </div>
        </div>
      `);
    });
  } catch (e) {
    console.error(e);
  }
});