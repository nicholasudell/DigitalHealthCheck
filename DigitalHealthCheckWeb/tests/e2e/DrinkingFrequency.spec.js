describe("Drinking frequency page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DrinkingFrequency';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DrinkingFrequency')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 2 of 7");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DrinkingFrequency')).toBeTruthy();
    })

    it("goes to AUDIT1 if monthly or less selected ", async () => {
        await page.click('#frequency', 'monthly');
        const checked = await page.isChecked('#frequency', 'monthly');
        expect(checked).toBeTruthy();
        await page.click('button:has-text("Continue")');
                // will be redirected to sex page
                await page.click('#sex-2', 'male');    
                await page.click('#identity', 'cis');          
                await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 3 of 7");
 
    }) 

    it("goes to AUDIT1 if 2 to 4 times per month selected ", async () => {
        // Click twotofourpermonth
        await page.click('#frequency-2', 'twotofourpermonth');
        const checked = await page.isChecked('#frequency-2', 'twotofourpermonth');
        expect(checked).toBeTruthy();
        await page.click('button:has-text("Continue")');
                // will be redirected to sex page
                await page.click('#sex-2', 'male');    
                await page.click('#identity', 'cis');          
                await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 3 of 7");
 
    }) 

    it("goes to AUDIT if 2 to 3 times per week selected ", async () => {
        // Click twotothreeperweek
        await page.click('#frequency-3', 'twotothreeperweek');
        const checked = await page.isChecked('#frequency-3', 'twotothreeperweek');
        expect(checked).toBeTruthy();
        await page.click('button:has-text("Continue")');
                // will be redirected to sex page
                await page.click('#sex-2', 'male');    
                await page.click('#identity', 'cis');          
                await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 3 of 7");
 
    }) 

    it("goes to AUDIT if 4 times or more per week selected ", async () => {
        // Click fourtimesweeklyplus
        await page.click('#frequency-4', 'fourtimesweeklyplus');
        const checked = await page.isChecked('#frequency-4', 'fourtimesweeklyplus');
        expect(checked).toBeTruthy();
        await page.click('button:has-text("Continue")');
                // will be redirected to sex page
                await page.click('#sex-2', 'male');    
                await page.click('#identity', 'cis');          
                await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 3 of 7");
 
    }) 

})