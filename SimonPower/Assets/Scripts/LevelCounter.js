// Este objeto requiere un GUIText que se vincula a guiText
@script RequireComponent (GUIText)
 
var sequence;
 
function Start() {
  // buscamos el componente Sequence del objeto Base para usarlo mas tarde
  sequence = GameObject.Find("Base").GetComponent(NewSequence);
}
 
// Por fin una de las funciones mas importantes de Unity, Update, esta se
// ejecuta en cada actualización puedes usarla para un montón de cosas
// nosotros la usaremos para actualizar el nivel en el que estamos
function Update () {
  guiText.text = "Level " + sequence.level();
}