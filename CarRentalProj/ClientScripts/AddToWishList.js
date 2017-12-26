function WishList(CarID, Details) {
    var LC = localStorage;
    LC.setItem(CarID, Details);
}