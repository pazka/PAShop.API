# PAShop.API Remarques

## Controllers surchargés ?

J'ai eu des problèmes de conception au niveau de la communication Controlleur/Service. 
Je n'arrivais pas à voir ou faire l'identification explicite des erreurs. Un service retournant souvent un objet ou une liste d'objet, ou renseigner une erreur dans l'exécution de la fonction ?
J'ai commencé à faire beaucoup de test de logique métier dans le contrôlleur, et utiliser les services pour faire des appels spéciaux au Repository.
J'ai finalement fini par décaler les responsabilités de chaque entité. 

## Un découplage faible au niveau des données spécifiques

J'aurais aimé créer un StockController pour la gestion du stock plutôt qu'avoir intégré quelques fonctions clefs dans Inventory et Items
J'ai aussi eu le sentiment que j'aurais dû créer des Helper à certains endroits plutôt que faire des opérations/transformations purement fonctionnelles et non-liées au métier. 

## RESThalf
Je ne suis pas convaincu par mes endpoints au niveau du stock, notamment à cause de l'orga du controlleur.
J'ai aussi eu du mal à adopter un contrat clair et consistant dans me retour de fonctions. Je ne pense pas que le retour de mes endpoints soient très pertinents (dans les StatusCodes ou les objets de retour en cas d'erreurs).


## Entity Framework et dbContext

Au niveau des includes et l'utilisation des DAO, je suis sûr de l'avoir mal utilisé dans 50% des cas. Par ex j'étais incertain du comportement d'un :
```
objA.idObjB = _objB.Id;

repo.Put(objA);
```
alors j'ajoutais la ligne 
``` objA.objB = _objB; ```

