// Necesitamos una fuente de audio para el fallo.
@script RequireComponent(AudioSource)
 
// Aqui guardaremos las notas
var seq_ary = new Array([]);
var seq_opts = 0; // número de notas == num de botones
var current = 0; // posición en la que estamos de la secuencia
var buttons; // un array con los GameObject de los botones
var playing = false; // Si estamos en medio de la reproducción de las notas
 
function Start () {
  // Buscamos todos los objetos que tengan nuestro script ButtonBehaviour
  buttons = GetComponentsInChildren( BotonBehaviourScript );
  seq_opts = buttons.length;
  audio.loop = true;
  audio.Stop();
  // Creamos la primera nota y la reproducimos
  createNote();
  seqPlay();
}
 
// Crea una nota al azar
function createNote () {
  print ("crea nota");
  seq_ary.Push( Random.Range( 0, seq_opts ) );
}
 
// Reproduce la secuencia actual
function seqPlay () {
  playing = true;
 
  print ("crea secuencia");
  // Esta construcción nos permite suspender el script para ejecutar los pasos
  // de la animación
  yield WaitForSeconds ( 1.1 );
  // iteramos por las notas y vamos haciendo sonar las notas
  for ( var n in seq_ary ) {
    buttons[n].changeButton( true );
    yield WaitForSeconds ( 0.4 );
    buttons[n].changeButton( false );
    yield WaitForSeconds ( 0.09 );
  }
  playing = false;
}
 
// Función que llamaran los botones al ser pulsados para comprobar el estado
// del juego.
function check ( obj ) {
  
  if ( !playing ) {
    // sigue a partir de --->>
    // si aun sigues interesado en este código es la peor forma de encontrar
    // un objeto en un array
    var c;
    var found = -1;
    for ( c = 0; c < buttons.length; c++ ) {
      print(buttons);
      print(obj);
      if ( buttons[c]==obj ) {
        found = c;
      }
    }
    print(found);
    // --->> En found tendremos el indice del botón que nos han pasado
    if ( found != -1 ) {
      // Si es la nota que esperamos
      if ( seq_ary[current] == found ) {
        // Subimos el apuntador
        current++;
        // Y si hemos terminado
        if ( current >= seq_ary.length ) {
          // Añadimos una nota, ponemos el apuntador al principio y
          // ejecutamos la secuencia.
          createNote();
          current = 0;
          seqPlay();
        }
        
      } else { // Si no es la nota
        print("aqui fijo");
        audio.Play(); // Reproducimos el sonido de error
        yield WaitForSeconds ( 0.4 ); // pausa
        audio.Stop();
        // Nuevo juego
        seq_ary = new Array([]);
        current = 0;
        createNote();
        seqPlay();
      }
    }
  }
}
 
// Indica el nivel en el que estamos
function level() {
  return seq_ary.length;
}