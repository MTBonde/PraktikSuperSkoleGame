using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORE.Scripts;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;




public class TowerManager : MonoBehaviour
{

    private int towerHeight;
  
    private GameObject[,] tower;

    private int rowToDelete=0;


    [SerializeField] GameObject brickPrefab;

    [SerializeField] GameObject canvasPrefab;

    [SerializeField] public List<Sprite> wrongImages;
    [SerializeField] public List<Sprite> wrongImages2;
    
    [SerializeField] Sprite image;

    private int towerRadius = 20;
    private int numberOfBricksInLane = 40;
  


    private List<Sprite> allImagesInCurrentRow;
    private List <BrickData> brickLanes;
    private int currentLane = 0;
    public bool correctAnswer = false;

    private Vector3 brickDimensions;

    private int amountOfOptions = 5;

    



    string currentQuestion;
    int currentQuestionIndex = 0;
    [SerializeField] TextMeshProUGUI displayBox;
    [SerializeField] GameObject imageHolerPrefab;
    string[] sentanses;
    int difficulty = 0;
    GameObject[,] tower;

    RawImage mainImgae;
    RawImage topImage;
    RawImage bottomImage;


    float towerRadius = 5f;
    int layerObjects = 14;
    // Start is called before the first frame update


   
    void Start()
    {
        brickLanes = new List<BrickData>()
        {
            new BrickData("hello",image,wrongImages),
            new BrickData("goodbye",image,wrongImages2)

        };

        //why is this here 2 times (here and in a function)?
      
        towerHeight = brickLanes.Count;

        allImagesInCurrentRow = brickLanes[currentLane].wrongImages;
        allImagesInCurrentRow.Add(brickLanes[currentLane].correctImage);
        brickDimensions = brickPrefab.GetComponent<MeshRenderer>().bounds.size;

        BuildTower();

        //updateDimensions = true;
    }

    void BuildTower()
    {
        tower = new GameObject[layerObjects, towerHeight];
        float nextAngle = 2 * Mathf.PI / layerObjects;
        float angle = 180.1f;
        Brick controller = brickPrefab.GetComponent<Brick>();

        for (int z = 0; z < towerHeight; z++)
        {
            int correctImageIndex = UnityEngine.Random.Range(0,towerWidth-1);
            for (int x = 0; x < layerObjects; x++)
            {
                float posX = Mathf.Cos(angle) * towerRadius;
                float posY = Mathf.Sin(angle) * towerRadius;
                Vector3 brickPos = transform.position + new Vector3(posX,z * 3,posY);
                if(z == 0 && x <= towerWidth)
                    controller.isShootable = true;
                else
                    controller.isShootable = false;
                if(x == correctImageIndex)
                    controller.isCorrect = true;
                else 
                    controller.isCorrect = false;

                tower[x, z] = Instantiate(brickPrefab, brickPos, quaternion.Euler(0,-angle,0));
                tower[x, z].transform.parent = transform;

                angle += nextAngle;
                if (x >= towerWidth-1) continue;

                if (x == correctImageIndex)
                    SetCorrectImage();
                else
                    SetRandomImage();
                GameObject imageholder = Instantiate(imageHolerPrefab, tower[x,z].transform);
                imageholder.transform.position = new(0,0,-0.51f);
            }
        }
    }


    // Update is called once per frame
    // The tower�checks if the right answer has been chosen and destroys the lowest tower lane. 
    void Update()
    {

        if (correctAnswer == true)
        {
            DestroyLowestTowerLane();

            correctAnswer = false;

        }
       
    }

    // the lowest tower lane is destroyed by knowing the numberOfBricksInLane and the accessing the 2d tower array that have all the bricks.
    // Lastly the whole tower is lowered the same amount as the height of a brick. 
    void DestroyLowestTowerLane()
    {

        if (rowToDelete < towerHeight)
        {

            for (int i = 0; i < numberOfBricksInLane; i++)
            {

                Destroy(tower[i, rowToDelete]);

            }




            rowToDelete++;
        }

            // sets the next rows pictures active and shows them. 


        if(rowToDelete<towerHeight)
        { 
            for (int i = 0; i < numberOfBricksInLane; i++)
            {
                if (i <= amountOfOptions - 1)
                {
                    tower[i, rowToDelete].transform.GetChild(0).gameObject.SetActive(true);

                    Brick brickComponent = tower[i, rowToDelete].GetComponent<Brick>();
                     brickComponent.isShootable = true;
                   
                }
            }


            gameObject.transform.Translate(0, -brickDimensions.y, 0);

        }
    }


    /// <summary>
    /// Method for building the tower based on numberOfBricksInLane, a list of brick data,towerHeight and radius. 
    /// The bricks are put into a 2D array with the parameteres x and z.
    /// x is the id of the brick in the x-axis. 
    /// z is the id of the height/lane the brick is in.  
    ///
    /// </summary>
    /// 
    void BuildTower()
    {

        //The tower angle is a value that represents the angle between the bricks and center of the tower. 
        //The start angle is an angle i chose based on where i want the tower to start build. 
        // The reason for this is so the first bricks in the 2d tower array is the ones used for displaying the pictures.

        float towerAngle = 2*Mathf.PI / numberOfBricksInLane;
        float startAngle = 180.3f;
        tower = new GameObject[numberOfBricksInLane, towerHeight];

        // the tower is built based on tower height and numberOfBricksInLane with a for loop. 
        for (int z = 0; z < towerHeight; z++)
        {

            // Random correct image index is used so the right answer is put randomly between the posible positions. 
            int correctImageIndex = UnityEngine.Random.Range(0, amountOfOptions);
            for (int x = 0; x < numberOfBricksInLane; x++)
            {

                //the new position of each brick is calculated based on angle,tower radius and the dimension of the brick. 
                //Calculating x and z with x=cos(v)*r and z=sin(v)*r
                // y is calculated based on the y dimension of the brick so the next lane is directly on top of the previus one.  
                Vector3 newPos = gameObject.transform.position+new Vector3(Mathf.Cos(startAngle) * towerRadius, z * brickDimensions.y, Mathf.Sin(startAngle) * towerRadius);

       

                // brick is the instantiated and the angle set as startangle. 
                // The brick is put into the 2d tower array. 
                // the brick is rotated so the picture would be facing the right way.
                // Then the towerobject is set as the parrent to the brick. 

                tower[x, z] = Instantiate(brickPrefab, newPos, quaternion.Euler(0,-startAngle,0));
                tower[x, z].transform.Rotate(new Vector3(0, -90, 0));
                tower[x, z].transform.parent = gameObject.transform;




                // The amount of options is a value that can be set based on difficulty if more potenial options is needed.
            
                if (x <= amountOfOptions-1)
                {
                    Brick brickComponent = tower[x, z].GetComponent<Brick>();
                    if (z == 0)
                    {
                        brickComponent.isShootable = true;
                    }
                    // The images are set here and instantiatetd on the right bricks. 
                    // and based on the value of correctImageIndex the right answer is set. 
                    if (x == correctImageIndex)
                    {

                        // The image for the brick and the correct image is given to the brick and can be used to check if the right brick is chosen.
                        brickComponent.sprite = image;
                        brickComponent.correctSprite = image;
                        
                       

                        canvasPrefab.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = image;
                         var instCanvas=Instantiate(canvasPrefab, tower[x, z].transform);

                        // makes all the other options besides those on the lowest lane not active. 
                        if(z!=0)
                        {
                            instCanvas.SetActive(false);
                        }

                       
                    }
                    else
                    {

                        brickComponent.sprite = brickLanes[currentLane].wrongImages[0];
                        brickComponent.correctSprite = image;
                        canvasPrefab.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = brickLanes[currentLane].wrongImages[0];
                        var instCanvas =Instantiate(canvasPrefab, tower[x, z].transform);

                        if (z != 0)
                        {
                            instCanvas.SetActive(false);
                        }


                    }

                }

                // startAngle is updated so the next brick gets placed further along the circle.

                startAngle += towerAngle;

            }
        }

    }




    
}
