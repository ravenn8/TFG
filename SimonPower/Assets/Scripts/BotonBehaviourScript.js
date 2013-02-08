// Necesitamos un recurso de audio que se nos mapea a "audio"
@script RequireComponent(AudioSource)

// Añadimos un propiedad para guardar la luz y no estar siempre haciendo búsquedas
var childLight : Light;
  
// La función Start se llama una vez al principio durante la vida del objeto.
function Start () {
  audio.loop = true;
  audio.Stop();
  
  // buscamos la luz y cambiamos al estado de desactivado por defecto
  childLight = GetComponentInChildren(Light);
  changeButton(false);
}
 
// Función que se llama cuando se pulsa un botón sobre el collider del objeto
function OnMouseDown () {
  audio.Play();
  changeButton(true);
}
 
// Función que se llama cuando se suelta un botón sobre el collider del objeto
function OnMouseUp () {
  audio.Stop();
  changeButton(false);
  
  // Hacemos una búsqueda global buscando el objeto Base y le enviamos el
  // mensaje "check" con el parámetro this, pure OO :`)
  GameObject.Find("Base").SendMessage('check',this);
}

function changeButton(val) {

  // Simplemente cambiamos el estado "enabled" de la luz
  childLight.enabled = val;
  
  if ( val ) {
    audio.Play();
    // Podemos acceder a la posición de los objetos a través de la propiedad transform
    transform.position.y -= 0.1;
  } else {
    audio.Stop();
    transform.position.y += 0.1;
  }
}


 
