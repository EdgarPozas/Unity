//Librerias por defecto
using UnityEngine;
using System.Collections;
//Libreria para importar elementos del canvas Text button Panel
using UnityEngine.UI;
//Libreria para poder cambiar de escena
using UnityEngine.SceneManagement;


//las clases se llaman igual que el archivo
public class ControlesJuego : MonoBehaviour {

	//Creamos 2 GameObjects para ahi guardar las camaras que ocupamos
	public GameObject camara_principal, camara_personaje;
	//Guardamos el diseño del disparo
	public GameObject disparo_prefab;
	//Creamos una variable para controlar la velocidad con la que nos movemos
	public int velocidad;
	//variable que representa el tiempo que tenemos para que llegar a la meta
	public int contador_tiempo;
	//Variable de tipo Text en la que tenemos el texto donde va a aparecer el tiempo
	public Text txt_tiempo;
	//Creamos una variable string para indicar el nombre del siguiente nivel
	public string siguiente_nivel;
	//Variable bool que solo contiene true o false y esta nos dira cuando se termino el juego
	public bool termino_juego;

	//Variables en las que vamos a tener los 2 mensajes por si gana o pierde
	public GameObject mensaje_siguiente_nivel;
	public GameObject mensaje_perdiste_nivel;

	//Metodo inicial el que se ejecuta al inicio del juego
	void Start(){
		//Metodo que hace que un funcion se repita segun sus parametros
		//el primer parametro "cuenta_atras" indica que va a ejecutar esa funcion
		//el segundo parametro 1 indica cuando va a empezar a ejecutarse, el tiempo es en segundos
		//el tercer parametro 1 indica cada cuando se va a repetir, el tiempo es en segundos
		//puedes checar en internet el funcionamiento de este metodo
		InvokeRepeating ("cuenta_atras",1,1);
	}

	//Este es la funcion en la cual le dijimos al InvokeRepeating  que se va a ejecutar cada segundo
	void cuenta_atras(){
		//hacemos una comprobacion de que si ya termino el juego ya no es necesario que siga la cuenta atras
		//si es una sola linea de codigo no es necesario poner las llaves, pero igual si las quieres poner no hay problema
		if (termino_juego)
			//con la palabra return indicamos que se termine de ejecutar la función que lo siguiente ya no se realize
			return;
		//si el juego no ha terminado entonces que el tiempo vaya decrementando
		contador_tiempo--;
	}

	//El metodo Update es el que hace que se actualize el juego
	void Update () {
		//verificamos que si se termino el juego ya no puedas moverte
		if (termino_juego)
			return;
		//Controles del juego estos ya los sabes
		if (Input.GetKey (KeyCode.W)) {
			//Adelante
			//vamos a indicar que nuestra posicion se le sume nuestra posicion hacia adelante
			//multiplicado por la velocidad a la que corre el juego
			//multiplicado por la velocidad que queramos que se mueva
			// transform.forward es hacia adelante
			transform.position += transform.forward*Time.deltaTime*velocidad;
		}
		if (Input.GetKey (KeyCode.S)) {
			//Atras
			//Si en vez de sumar le restamos iremos hacia el lado contrario, en este caso iremos
			//hacia atras
			transform.position -= transform.forward*Time.deltaTime*velocidad;
		}
		if (Input.GetKey (KeyCode.D)) {
			//Derecha
			//transform.right de nuestra posicion nos moveremos hacia la derecha
			transform.position += transform.right*Time.deltaTime*velocidad;
		}
		if (Input.GetKey (KeyCode.A)) {
			//Izquierda
			//Si le restamos transform.right iremos a la izquierda
			transform.position -= transform.right*Time.deltaTime*velocidad;
		}

		//Cambio de Camaras
		if (Input.GetKey (KeyCode.Q)) {
			//vista principal
			//activamos la principal y desactivamos a la del personaje
			camara_principal.SetActive(true);
			camara_personaje.SetActive(false);
		}
		if (Input.GetKey (KeyCode.E)) {
			//vista primera persona
			//activamos la del personaj y desactivamos la principal
			camara_personaje.SetActive(true);
			camara_principal.SetActive(false);
		}

		//Disparar
		//usamos keydown para que se creen de uno en uno
		if (Input.GetKeyDown (KeyCode.Space)) {
			//creamos el disparo
			//Instantiate(objeto,posicion,rotacion)
			//pasamos el prefab
			//indicamos que se cree donde estamos pero mas arriba
			GameObject disparo = Instantiate (disparo_prefab, transform.position + new Vector3 (0, 4, 0), transform.rotation) as GameObject;
			//Accedemos a las fisicas del disparo GetComponent<Rigidbody>() y le añadirmos una fuerza
			//la fuerza sera hacia adelante transform.forward*10,
			//y el segundo parametro tipo de fuerza sera de velocidad
			disparo.GetComponent<Rigidbody>().AddForce (transform.forward*10,ForceMode.VelocityChange);
			//Destruimos el disparo despues de 2 segundo, para no saturar la escena de bolitas
			Destroy (disparo, 2);
		}

		//movimiento del jugador
		//con esta linea indicamos que rote el objeto segun la posicion de la camara
		//a donde apunte el mouse apuunte el jugador
		transform.rotation=Quaternion.Euler(new Vector3(0,Input.mousePosition.x,0));

		//si se acabo el tiempo, si es menor o igual a cero 
		if (contador_tiempo <= 0) {
			//a la variable le decimos que termino
			termino_juego = true;
			//mostramos el mensaje del nivel
			//activamos el meensaje
			mensaje_perdiste_nivel.SetActive (true);
		}
		//en donde se muestra el tiempo de juego ponemos que aparesca asi
		//Tiempo restante : 10
		txt_tiempo.text = "Tiempo restante : " + contador_tiempo;

	}
	//Con esto obtenemos las colisiones de los objetos
	void OnCollisionEnter(Collision collision){
		//creamos una variable auxiliar para la colision
		Collider col = collision.collider;
		//Le preguntamos al juego que si choco con el portal
		//el nombre debe ser el mismo que el que tengas en el hierarchy
		//y es importante que el objeto tenga un boxCollider para la colision
		if (col.name.Equals ("Portal (Cubo rojo)")) {
			//Si llegamos a la meta indicamos que termino el juego
			termino_juego = true;
			//mostramos el mensaje de siguiente nivel
			//lo activamos
			mensaje_siguiente_nivel.SetActive (true);
		}
	}
	//Funcion que se ejecuta cuando le damos click al boton siguiente
	public void btnsiguiente(){
		//cargas la escena
		SceneManager.LoadScene (siguiente_nivel);
	}
	//funcion que se ejecuta cuando le damos click al boton de reiniciar
	public void btnreiniciar(){
		//cargas esta misma escena
		//se reinicia
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}
}
